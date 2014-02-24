﻿using System;
using System.Net;
using System.Transactions;
using System.Web.Http;
using EnergyTrading.MDM.MappingService2.Filters;
using EnergyTrading.MDM.MappingService2.Infrastructure;
using EnergyTrading.MDM.MappingService2.Infrastructure.Controllers;
using EnergyTrading.MDM.MappingService2.Infrastructure.ETags;
using EnergyTrading.MDM.MappingService2.Infrastructure.Results;
using EnergyTrading.MDM.Messages;
using EnergyTrading.MDM.Services;
using RWEST.Nexus.MDM.Contracts;

namespace EnergyTrading.MDM.MappingService2.Controllers
{
    public class EntityMapController<TContract, TEntity> : BaseEntityController
        where TContract : class, IMdmEntity
        where TEntity : IEntity
    {
        protected IMdmService<TContract, TEntity> service;

        public EntityMapController(IMdmService<TContract, TEntity> service)
        {
            this.service = service;
        }

        [ETagChecking]
        public IHttpActionResult Get([IfNoneMatch] ETag etag)
        {
            var request = MessageFactory.MappingRequest(QueryParameters);
            request.Version = etag.ToVersion();

            ContractResponse<TContract> response;
            using (var scope = new TransactionScope(TransactionScopeOption.Required, ReadOptions()))
            {
                response = this.service.Map(request);
                scope.Complete();
            }

            if (response.IsValid)
            {
                return new ResponseWithETag<TContract>(Request, response.Contract, HttpStatusCode.OK, response.Version);
            }
            
            // THROW FAULTFACTORY EXCEPTION
            throw new Exception("Undefined exception to be fixed");
        }
    }
}
