using System.Collections.Generic;
using Containers;
using Containers.Modificators;
using Core;
using Logic.Idle.Workers;
using Logic.Model;
using UniRx;

namespace Logic.Profile
{
    public class ProfileClient : IReadOnlyProfile
    {
        public struct Ctx
        {
            public int workerCountForMerge;
            public int maxGrade;
        }

        private readonly Ctx _ctx;
        
        public IReactiveProperty<int> Moneys { get; }
        public IReactiveCollection<WorkerModel> Workers { get; }
        public IReactiveCollection<BuildingModel> Buildings { get; }
        public IReactiveCollection<ModificatorInfo> Modificators { get; }
        public IReactiveProperty<BuildProgressModel> CurrentBuilding { get; }
        public IReactiveProperty<Scenes> CurrentScene { get; }
        public IReactiveProperty<int> CurrentEffectiency { get; }
        public IReactiveProperty<float> CurrentTimeSpeed { get; }
        public IReactiveProperty<int> CurrentEffectiencyLevel { get; }
        public IReactiveProperty<int> CurrentTimeSpeedLevel { get; }
        public IReactiveProperty<int> CurrentMergeLevel { get; }

        public UpgradeModel UpgradeModel { get; }
        private Dictionary<int, List<WorkerModel>> _workerGrades = new();
        private int _cashWorkersCount
        {
            get
            {
                int count = 0;
                foreach (var workerGrade in _workerGrades)
                {
                    count += workerGrade.Value.Count;
                }

                return count;
            }    
        }

        public ProfileClient(Ctx ctx)
        {
            _ctx = ctx;

            Moneys = new ReactiveProperty<int>();
            Workers = new ReactiveCollection<WorkerModel>();
            Modificators = new ReactiveCollection<ModificatorInfo>();
            Buildings = new ReactiveCollection<BuildingModel>();
            CurrentScene = new ReactiveProperty<Scenes>();
            CurrentBuilding = new ReactiveProperty<BuildProgressModel>();
            CurrentEffectiencyLevel = new ReactiveProperty<int>();
            CurrentTimeSpeedLevel = new ReactiveProperty<int>();
            CurrentMergeLevel = new ReactiveProperty<int>();
            UpgradeModel = new UpgradeModel();
            CurrentEffectiency = new ReactiveProperty<int>();
            CurrentTimeSpeed = new ReactiveProperty<float>();
        }

        public bool CanMerge(out int grade)
        {
            if(_cashWorkersCount != Workers.Count)
                CalcWorkerGrades();
            
            grade = 0;
            foreach (var workerGrade in _workerGrades)
            {
                if (workerGrade.Value.Count > _ctx.workerCountForMerge)
                {
                    grade = workerGrade.Key;
                    return true;
                }
            }

            return false;
        }

        public List<WorkerModel> GetWorkersForMerge(int grade)
        {
            if (!CanMerge(out int canGrade))
                return null;

            if (canGrade != grade)
                return null;

            List<WorkerModel> result = new List<WorkerModel>();
            for (int i = 0; i < _ctx.workerCountForMerge; i++)
            {
                result.Add(_workerGrades[grade][i]);
            }

            return result;
        }

        private void CalcWorkerGrades()
        {
            foreach (var worker in Workers)
            {
                if(worker.Grade.Value >= _ctx.maxGrade)
                    continue;
                if (!_workerGrades.ContainsKey(worker.Grade.Value))
                    _workerGrades[worker.Grade.Value] = new List<WorkerModel>();
                
                _workerGrades[worker.Grade.Value].Add(worker);
            }
        }
    }
}