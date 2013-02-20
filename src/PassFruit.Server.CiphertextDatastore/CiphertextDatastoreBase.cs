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

        public abstract CipheredAccountDto Get(Guid accountId);

        internal protected abstract void InternalSave(CipheredAccountDto cipheredAccountDto);

        public IEnumerable<CipheredAccountDto> GetAll(CipheredAccountStatus cipheredAccountStatus = CipheredAccountStatus.Active)
        {
            return GetAllIds().Select(Get).Where(cipheredAccountDto =>
                IsMatchedByStatus(cipheredAccountStatus, cipheredAccountDto.Deleted));
        }

        public void Save(CipheredAccountDto cipheredAccountDto)
        {
            if (cipheredAccountDto.IsNew())
            {
                cipheredAccountDto.Id = Guid.NewGuid();
            }
            InternalSave(cipheredAccountDto);
        }
        
        public void Delete(Guid accountId)
        {
            var deletedCipheredAccount = new CipheredAccountDto(accountId, true);
            Save(deletedCipheredAccount);
        }

        public void DeleteAll()
        {
            var allAccountIds = GetAllIds();
            foreach (var accountId in allAccountIds)
            {
                Delete(accountId);
            }
        }

        private static bool IsMatchedByStatus(CipheredAccountStatus cipheredAccountStatus, bool isDeleted)
        {
            switch (cipheredAccountStatus)
            {
                case CipheredAccountStatus.Any:
                    return true;
                case CipheredAccountStatus.Active:
                    return !isDeleted;
                case CipheredAccountStatus.Deleted:
                    return isDeleted;
                default:
                    throw new NotSupportedException("The account status filter specified is not supported: " + cipheredAccountStatus);
            }
        }

    }

}
