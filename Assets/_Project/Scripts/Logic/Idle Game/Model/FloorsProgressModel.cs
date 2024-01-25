using System.Collections.Generic;
using UnityEngine;

namespace Logic.Model
{
    public class FloorsProgressModel
    {
        public List<Vector3> setFloors = new(); // поставленные этажи для сохранения, если поставил этаж, то записываю сюда
        public int reward; // итоговая награда 
    }
}