namespace PassFruit.Security
{
    public class EncryptedDataDto
    {
        
        public byte[] Salt { get; set; }
        
        public byte[] InitializationVector { get; set; }
        
        public int Iterations { get; set; }

        public byte[] Ciphertext { get; set; }

    }
}
