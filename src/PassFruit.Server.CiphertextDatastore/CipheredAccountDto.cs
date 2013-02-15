using System;
using System.Linq;

namespace PassFruit.Server.CiphertextDatastore
{
    public class CipheredAccountDto
    {

        public CipheredAccountDto()
        {
            Id = Guid.Empty;
            Ciphertext = new byte[0];
            InitializationVector = new byte[0];
            Salt= new byte[0];
        }

        internal CipheredAccountDto(Guid id, bool isDeleted = false): this()
        {
            Id = id;
            Deleted = isDeleted;
        }

        public Guid Id { get; set; }  // Setter should be private but Deserialization needs it public

        public byte[] Ciphertext { get; set; }

        public byte[] InitializationVector { get; set; }

        public byte[] Salt { get; set; }

        public bool Deleted { get; set; }  // Setter should be private but Deserialization needs it public

        public bool IsNew()
        {
            return Id == Guid.Empty;
        }

        public override string ToString()
        {
            return Id.ToString() + " - Ciphered";
        }

        protected bool Equals(CipheredAccountDto other)
        {
            return Id.Equals(other.Id) 
                && Ciphertext.SequenceEqual(other.Ciphertext) 
                && InitializationVector.SequenceEqual(other.InitializationVector) 
                && Salt.SequenceEqual(other.Salt) 
                && Deleted.Equals(other.Deleted);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CipheredAccountDto)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Ciphertext != null ? Ciphertext.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (InitializationVector != null ? InitializationVector.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Salt != null ? Salt.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Deleted.GetHashCode();
                return hashCode;
            }
        }

    }
}