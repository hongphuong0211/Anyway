using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceTable<T> {
    /// <summary>
    /// The table.
    /// </summary>
    public ChanceValue<T>[] table;
    /// <summary>
    /// The total.
    /// </summary>
    public int total;
    /// <summary>
    /// Initializes a new instance of the <see cref="DataStats.ChanceTable"/> class.
    /// </summary>
    public ChanceTable() {
        this.table = new ChanceValue<T>[0];
    }
    /// <summary>
    /// Adds the item.
    /// </summary>
    /// <param name="id">Identifier.</param>
    /// <param name="chance">Chance.</param>
    public void AddItem(T id, int chance) {
        if (chance <= 0) return;
        bool isNewValue = true;
        for (int i = 0; i < table.Length; i++) {
            ChanceValue<T> ctb = table[i];
            if (EqualityComparer<T>.Default.Equals(ctb.id, id)) {
                ctb.AddChance(chance);
                isNewValue = false;
            }
        }
        List<ChanceValue<T>> list = new List<ChanceValue<T>>();
        for (int i = 0; i < table.Length; i++) {
            list.Add(table[i]);
        }
        if (isNewValue) {
            list.Add(new ChanceValue<T>(id, chance));
        }
        this.table = list.ToArray();
        this.total = 0;
        ChanceValue<T>[] array = this.table;
        for (int i = 0; i < array.Length; i++) {
            ChanceValue<T> chanceValue = array[i];
            this.total += chanceValue.chance;
        }
    }
    /// <summary>
    /// Gets the item.
    /// </summary>
    /// <returns>The item.</returns>
    /// <param name="no">No.</param>
    public T GetItem(int no) {
        int num = 0;
        ChanceValue<T>[] array = this.table;
        for (int i = 0; i < array.Length; i++) {
            ChanceValue<T> chanceValue = array[i];
            num += chanceValue.chance;
            if (no < num) {
                return chanceValue.id;
            }
        }
        return default(T);
    }

    public T GetRandomItem() {
        int r = Random.Range(0, this.total);
        //Debug.Log("Random " + r);
        return GetItem(r);
    }
    public void ClearTable() {
        this.total = 0;
        this.table = new ChanceValue<T>[0];
    }
    public void RemoveItem(T item) {
        ChanceValue<T>[] temp = this.table;
        ClearTable();
        for (int i = 0; i < temp.Length; i++) {
            ChanceValue<T> cv = temp[i];
            AddItem(cv.id, cv.chance);
        }
    }
    public void CopyTable(ChanceTable<T> otherTbl) {
        ClearTable();
        for (int i = 0; i < otherTbl.table.Length; i++) {
            ChanceValue<T> cv = otherTbl.table[i];
            AddItem(cv.id, cv.chance);
        }
    }
    public int GetValue(T id) {
        int valueChance = 0;
        for (int i = 0; i < table.Length; i++) {
            ChanceValue<T> cv = table[i];
            if (EqualityComparer<T>.Default.Equals(cv.id, id)) {
                valueChance += cv.chance;
            }
        }
        return valueChance;
    }
    public float GetPercentage(T id) {
        int valueChance = 0;
        for (int i = 0; i < table.Length; i++) {
            ChanceValue<T> cv = table[i];
            if (EqualityComparer<T>.Default.Equals(cv.id, id)) {
                valueChance += cv.chance;
            }
        }
        float percent = (float)valueChance * 100f / (float)this.total;
        return percent;
    }
}
public class ChanceValue<T> {
    /// <summary>
    /// The chance.
    /// </summary>
    public int chance;
    /// <summary>
    /// The identifier.
    /// </summary>
    public T id;
    /// <summary>
    /// Initializes a new instance of the <see cref="DataStats.ChanceValue"/> class.
    /// </summary>
    /// <param name="id">Identifier.</param>
    /// <param name="chance">Chance.</param>
    public ChanceValue(T id, int chance) {
        this.chance = chance;
        this.id = id;
    }
    public void AddChance(int chance) {
        this.chance += chance;
    }
}