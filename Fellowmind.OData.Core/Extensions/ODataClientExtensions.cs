using System;
using System.Linq;
using Microsoft.OData.Client;

namespace Fellowmind.OData.Core.Extensions
{
    public static class ODataClientExtensions
    {
        public static DataServiceQuery<TEntity> ToODataDTS<TEntity>(this IQueryable<TEntity> query) where TEntity : BaseEntityType
        {
            return (DataServiceQuery<TEntity>)query;
        }

        public static Guid ConvertCrmUriToGuid(string uri)
        {
            return Guid.Parse(uri.Substring(uri.Length - 37, 36));
        }

        public static TEntityType GetEntityReference<TEntityType>(DataServiceContext crmResources, Guid entityId) where TEntityType : BaseEntityType
        {
            object entity = null;
            Type entityType = typeof(TEntityType);

            // Get already tracked entities that are type of TEntityType
            var trackedEntities = crmResources.Entities.Where(e => e.Entity is TEntityType);

            // Get key property of this entity (eg. for entity of type ecr_route this would be ecr_routeid)
            var keyNames = entityType.GetAttributeValue((KeyAttribute keyattr) => keyattr.KeyNames);
            if (keyNames.Count != 1)
            {
                throw new Exception("Cannot make entity reference since the key of this entity is not formed from one parameter.");
            }
            string keyPropName = entityType.GetAttributeValue((KeyAttribute keyAttr) => keyAttr.KeyNames.First());
            var keyProp = entityType.GetProperty(keyPropName);

            entity = trackedEntities.FirstOrDefault(e => (Guid)keyProp.GetValue(e.Entity, null) == entityId)?.Entity;

            // Entity is not already tracked so we will create new instance of the entity and set its entity id and attach it to the entity tracker
            if (entity == null)
            {
                entity = Activator.CreateInstance(entityType);

                keyProp.SetValue(entity, entityId, null);

                var entitySetName = entityType.GetAttributeValue((EntitySetAttribute esa) => esa.EntitySet);
                crmResources.AttachTo(entitySetName, entity);
            }

            return (TEntityType)entity;
        }

        public static Guid? GetEntityIdFromTrackingContext(DataServiceContext crmResources, object entity)
        {
            var trackedEntity = crmResources.Entities.FirstOrDefault(e => e.Entity == entity);
            if (trackedEntity == null)
            {
                return null;
            }
            var entityIdentityLocalPath = trackedEntity.Identity.LocalPath;
            var regexObj = new System.Text.RegularExpressions.Regex(@"\((.{36})\)");
            var matchResult = regexObj.Match(entityIdentityLocalPath);
            if (matchResult.Success)
            {
                var id = Guid.Parse(matchResult.Groups[1].Value);
                return id;
            }
            else
            {
                return null;
            }
        }
    }
}
