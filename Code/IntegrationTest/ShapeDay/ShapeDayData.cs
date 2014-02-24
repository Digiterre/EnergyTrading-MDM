namespace EnergyTrading.MDM.Test
{
    using System;
    using System.Linq;

    using EnergyTrading;
    using EnergyTrading.Contracts.Search;
    using EnergyTrading.Data.EntityFramework;
    using RWEST.Nexus.MDM.Contracts;
    using EnergyTrading.MDM.Extensions;
    using EnergyTrading.Search;

    using DateRange = EnergyTrading.DateRange;
    using ShapeDay = EnergyTrading.MDM.ShapeDay;
    using SourceSystem = EnergyTrading.MDM.SourceSystem;

    public static class ShapeDayData
    {
        private static readonly DbSetRepository repository;

        private static DateTime baseDate;

        static ShapeDayData()
        {
            repository = ObjectScript.Repository;
        }

        public static ShapeDay CreateBasicEntity()
        {
            var entity = ObjectMother.Create<ShapeDay>();
            repository.Add(entity);
            repository.Flush();
            return entity;
        }

        public static ShapeDay CreateBasicEntityWithOneMapping()
        {
            SourceSystem endur = repository.Queryable<SourceSystem>().Where(system => system.Name == "Endur").First();

            var entity = ObjectMother.Create<ShapeDay>();

            var endurMapping = new ShapeDayMapping
                {
                    MappingValue = Guid.NewGuid().ToString(), 
                    System = endur, 
                    IsDefault = true, 
                    Validity = new DateRange(DateTime.MinValue, DateTime.MaxValue.Subtract(new TimeSpan(72, 0, 0)))
                };

            entity.ProcessMapping(endurMapping);
            repository.Add(entity);
            repository.Flush();

            return entity;
        }

        public static RWEST.Nexus.MDM.Contracts.ShapeDay CreateContractForEntityCreation()
        {
            var contract = new RWEST.Nexus.MDM.Contracts.ShapeDay();
            AddDetailsToContract(contract);
            return contract;
        }

        public static ShapeDay CreateEntityWithTwoDetailsAndTwoMappings()
        {
            SourceSystem endur = repository.Queryable<SourceSystem>().Where(system => system.Name == "Endur").First();
            SourceSystem trayport =
                repository.Queryable<SourceSystem>().Where(system => system.Name == "Trayport").First();

            var entity = new ShapeDay();
            baseDate = DateTime.Today.Subtract(new TimeSpan(72, 0, 0));
            SystemTime.UtcNow = () => new DateTime(DateTime.Today.Subtract(new TimeSpan(73, 0, 0)).Ticks);

            AddDetailsToEntity(entity, DateTime.MinValue, baseDate);
            AddDetailsToEntity(entity, baseDate, DateTime.MaxValue);

            SystemTime.UtcNow = () => DateTime.Now;

            var trayportMapping = new ShapeDayMapping
                {
                    MappingValue = Guid.NewGuid().ToString(), 
                    System = trayport, 
                    Validity = new DateRange(DateTime.MinValue, DateTime.MaxValue)
                };

            var endurMapping = new ShapeDayMapping
                {
                    MappingValue = Guid.NewGuid().ToString(), 
                    System = endur, 
                    IsDefault = true, 
                    Validity = new DateRange(DateTime.MinValue, DateTime.MaxValue)
                };

            entity.ProcessMapping(trayportMapping);
            entity.ProcessMapping(endurMapping);

            repository.Add(entity);
            repository.Flush();
            return entity;
        }

        public static void CreateSearch(Search search, ShapeDay entity1, ShapeDay entity2)
        {
            CreateSearchData(search, entity1, entity2);
        }

        public static RWEST.Nexus.MDM.Contracts.ShapeDay MakeChangeToContract(RWEST.Nexus.MDM.Contracts.ShapeDay currentContract)
        {
            AddDetailsToContract(currentContract);
            currentContract.Nexus.StartDate = currentContract.Nexus.StartDate.Value.AddDays(2);
            return currentContract;
        }

        private static void AddDetailsToContract(RWEST.Nexus.MDM.Contracts.ShapeDay contract)
        {
            var entity = ObjectMother.Create<ShapeDay>();
            repository.Add(entity);
            repository.Flush();

            contract.Details = new ShapeDayDetails()
            {
                DayType = entity.DayType,
                Shape = entity.Shape.CreateNexusEntityId(() => entity.Shape.Name),
                ShapeElement = entity.ShapeElement.CreateNexusEntityId(() => entity.ShapeElement.Name)
            };
        }

        private static void AddDetailsToEntity(ShapeDay entity, DateTime startDate, DateTime endDate)
        {
            var newEntity = ObjectMother.Create<ShapeDay>();
            entity.DayType = newEntity.DayType;
            entity.Shape = newEntity.Shape;
            entity.ShapeElement = newEntity.ShapeElement;
        }

        private static void CreateSearchData(Search search, ShapeDay entity1, ShapeDay entity2)
        {
            search.AddSearchCriteria(SearchCombinator.Or)
                .AddCriteria("Id", SearchCondition.NumericEquals, entity1.Id.ToString())
                .AddCriteria("Id", SearchCondition.NumericEquals, entity2.Id.ToString());
        }
    }
}
