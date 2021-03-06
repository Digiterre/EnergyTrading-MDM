namespace EnergyTrading.MDM
{
    using System;
    using System.Collections.Generic;

    using EnergyTrading;
    using EnergyTrading.Data;
    using EnergyTrading.MDM.Extensions;

    public partial class SourceSystem : IIdentifiable, IEntity, IEntityDetail
    {
        public SourceSystem()
        {
            this.Mappings = new List<SourceSystemMapping>();
            this.Validity = new DateRange();
            this.Timestamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };

            this.OnCreate();
        }

        /// <summary>
        /// Allow for construction actions in the partial class.
        /// </summary>
        partial void OnCreate();
        
        public int Id { get; set; }

        object IIdentifiable.Id
        {
            get { return this.Id; }
        }

        public virtual IList<SourceSystemMapping> Mappings { get; private set; }

        IEntity IRangedChild.Entity
        {
            get { return this; }
            set { }
        }

        public DateRange Validity { get; set; }

        /// <summary>
        /// Gets or sets the Timestamp property.
        /// <para>
        /// This is implemented by a RowVersion property on the database.
        /// </para>
        /// </summary>
        public byte[] Timestamp { get; set; }

        /// <summary>
        /// Gets the version property.
        /// <para>
        /// This is the Timestamp property converted to a long, only equality operations
        /// should be performed with this value as relative difference has no business meaning.
        /// </para>
        /// </summary>
        public ulong Version
        {
            get
            {
                var version = this.Timestamp.ToUnsignedLongVersion();
                version = this.Mappings.LatestVersion(version);
                return version;
            }
        }

        /// <summary>
        /// Add a details to the SourceSystem checking its validity 
        /// </summary>
        /// <param name="details"></param>
        public void AddDetails(SourceSystem details)
        {
            // Sanity checks
            if (details == null)
            {
                throw new ArgumentNullException("details");
            }

            // Copy the bits across
            CopyDetails(details);
            this.Validity = details.Validity;

            // Trim all the mappings that extend past the end of the entity.
            this.Mappings.TrimMappings(this.Validity.Finish);
        }

        /// <summary>
        /// Perform the field by field copy operation
        /// </summary>
        partial void CopyDetails(SourceSystem details);

        /// <summary>
        /// Add or update a mapping, checking that it exists and that the details are compatible.
        /// </summary>
        /// <param name="mapping"></param>
        public void ProcessMapping(SourceSystemMapping mapping)
        {
            this.ProcessMapping(this.Mappings, mapping, this.Validity.Finish);      
        }

        void IEntity.AddDetails(IEntityDetail details)
        {
            this.AddDetails(details as SourceSystem);
        }

        void IEntity.ProcessMapping(IEntityMapping mapping)
        {
            this.ProcessMapping(mapping as SourceSystemMapping);
        }
    }
}