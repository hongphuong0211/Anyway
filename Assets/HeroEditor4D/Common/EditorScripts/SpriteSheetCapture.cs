using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.CharacterScripts;
using Assets.HeroEditor4D.Common.CommonScripts;
using UnityEngine;

namespace Assets.HeroEditor4D.Common.EditorScripts
{
    [RequireComponent(typeof(Camera))]
    public class SpriteSheetCapture : MonoBehaviour
    {
        private Character4D _character;

        public void Capture(Vector2 direction, List<CaptureOption> options, int frameSize, int frameCount, bool shadow)
        {
            StartCoroutine(CaptureFrames(direction, options, frameSize, frameCount, shadow));
        }

        private IEnumerator CaptureFrames(Vector2 direction, List<CaptureOption> options, int frameSize, int frameCount, bool shadow)
        {
            _character = FindObjectOfType<Character4D>();
            _character.SetDirection(direction);
            _character.Shadows.ForEach(i => i.SetActive(false));
            _character.Shadows[0].SetActive(shadow);

            var stateHandler = _character.Animator.GetBehaviours<StateHandler>().SingleOrDefault(i => i.Name == "Death");

            if (stateHandler)
            {
                stateHandler.StateExit.RemoveAllListeners();
            }

            var clips = new Dictionary<string, List<Texture2D>>();

            foreach (var option in options)
            {
                _character.Animator.SetInteger("State", (int) option.State);

                if (option.Action != null)
                {
                    _character.Animator.SetTrigger(option.Action); // TODO: This doesn't work. Need to find a solution of how to pass the action to ShowFrame.
                }
                else
                {
                    _character.Animator.ResetTrigger("Slash1H");
                    _character.Animator.ResetTrigger("Slash2H");
                    _character.Animator.ResetTrigger("Jab");
                }

                _character.Animator.SetBool("Action", option.Action != null);
                _character.Animator.speed = 2;

                yield return new WaitForSeconds(0.1f);

                _character.Animator.speed = 0;

                var lowerClip = option.State == CharacterState.Death ? null : _character.Animator.GetCurrentAnimatorClipInfo(0)[0].clip;
                var upperClip = option.State == CharacterState.Death ? null : _character.Animator.GetCurrentAnimatorClipInfo(1)[0].clip;
                var complexClip = option.State == CharacterState.Death ? _character.Animator.GetCurrentAnimatorClipInfo(2)[0].clip : null;
                
                for (var j = 0; j < frameCount; j++)
                {
                    var normalizedTime = (float) j / (frameCount - 1);
                    var clip = upperClip ?? complexClip;
                    var expressionEvent = clip.events.Where(i => i.functionName == "SetExpression" && Mathf.Abs(i.time / clip.length - normalizedTime) <= 1f / (frameCount - 1))
                        .OrderBy(i => Mathf.Abs(i.time / clip.length - normalizedTime)).FirstOrDefault();

                    if (expressionEvent != null)
                    {
                        _character.SetExpression(expressionEvent.stringParameter);
                    }

                    yield return ShowFrame(lowerClip?.name, upperClip?.name, complexClip?.name, normalizedTime);

                    var frame = CaptureFrame(frameSize, frameSize);
                    var animationName = option.Action ?? option.State.ToString();

                    if (clips.ContainsKey(animationName))
                    {
                        clips[animationName].Add(frame);
                    }
                    else
                    {
                        clips.Add(animationName, new List<Texture2D> { frame });
                    }
                }
            }

            _character.LayerManager.Sprites[0].gameObject.SetActive(false);
            _character.AnimationManager.SetState(CharacterState.Idle);
            _character.Animator.speed = 1;

            if (stateHandler)
            {
                stateHandler.StateExit.AddListener(() => _character.SetExpression("Default"));
            }

            var texture = CreateSheet(clips, frameSize, frameSize);

            yield return StandaloneFilePicker.SaveFile("Save as sprite sheet", "", "Character", "png", texture.EncodeToPNG(), (success, path) => { Debug.Log(success ? $"Saved as {path}" : "Error saving."); });
        }

        private IEnumerator ShowFrame(string lowerClip, string upperClip, string complexClip, float normalizedTime)
        {
            if (lowerClip != null && lowerClip.EndsWith("L")) lowerClip = lowerClip.Substring(0, lowerClip.Length - 1);
            if (upperClip != null && upperClip.EndsWith("U")) upperClip = upperClip.Substring(0, upperClip.Length - 1);

            if (lowerClip != null) _character.Animator.Play(lowerClip, 0, normalizedTime);
            if (upperClip != null) _character.Animator.Play(upperClip, 1, normalizedTime);
            if (complexClip != null) _character.Animator.Play(complexClip, 2, normalizedTime);
            
            yield return null;

            while (_character.Animator.GetCurrentAnimatorClipInfo(complexClip == null ? 0 : 2).Length == 0)
            {
                yield return null;
            }

            if (_character.Animator.IsInTransition(1))
            {
                Debug.Log("IsInTransition");
            }
        }

        private Texture2D CaptureFrame(int width, int height)
        {
            var cam = GetComponent<Camera>();
            var renderTexture = new RenderTexture(width, height, 24);
            var texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);

            cam.targetTexture = renderTexture;
            cam.Render();
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            cam.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            return texture2D;
        }

        private Texture2D CreateSheet(Dictionary<string, List<Texture2D>> clips, int width, int height)
        {
            var texture = new Texture2D(clips.First().Value.Count * width, clips.Keys.Count * height);

            foreach (var clip in clips)
            {
                for (var i = 0; i < clip.Value.Count; i++)
                {
                    texture.SetPixels(i * width, clips.Keys.Reverse().ToList().IndexOf(clip.Key) * height, width, height, clip.Value[i].GetPixels());
                }
            }

            texture.Apply();

            return texture;
        }
    }

    public class CaptureOption
    {
        public CharacterState State;
        public string Action;

        public CaptureOption(CharacterState state, string action)
        {
            State = state;
            Action = action;
        }
    }
}