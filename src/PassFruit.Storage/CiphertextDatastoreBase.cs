using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassFruit.Server.CiphertextDatastore
{

    public abstract class CiphertextDatastoreBase
    {
        protected readonly Guid UserId;

        public CiphertextDatastoreBase(Guid userId)
        {
            UserId = userId;
        }

        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract IEnumerable<Guid> GetAllIds();

        public abstract CiphertextDto Get(Guid accountId);

        internal protected abstract void InternalSave(CiphertextDto ciphertextDto);

        public IEnumerable<CiphertextDto> GetAll(CiphertextStatus ciphertextStatus = CiphertextStatus.Active)
        {
            return GetAllIds().Select(Get).Where(ciphertextDto =>
                IsMatchedByStatus(ciphertextStatus, ciphertextDto.Deleted));
        }

        public void Save(CiphertextDto ciphertextDto)
        {
            if (ciphertextDto.IsNew())
            {
                ciphertextDto.Id = Guid.NewGuid();
            }
            InternalSave(ciphertextDto);
        }
        
        public void Delete(Guid accountId)
        {
            var deletedCiphertextDto = new CiphertextDto(accountId, true);
            Save(deletedCiphertextDto);
        }

        public void DeleteAll()
        {
            var allAccountIds = GetAllIds();
            foreach (var accountId in allAccountIds)
            {
                Delete(accountId);
            }
        }

        private static bool IsMatchedByStatus(CiphertextStatus ciphertextStatus, bool isDeleted)
        {
            switch (ciphertextStatus)
            {
                case CiphertextStatus.Any:
                    return true;
                case CiphertextStatus.Active:
                    return !isDeleted;
                case CiphertextStatus.Deleted:
                    return isDeleted;
                default:
                    throw new NotSupportedException("The account status filter specified is not supported: " + ciphertextStatus);
            }
        }

    }

}
