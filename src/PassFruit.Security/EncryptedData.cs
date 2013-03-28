namespace PassFruit.Security
{
    public class EncryptedData
    {

        public EncryptedData(byte[] salt, byte[] initializationVector, int iterations, byte[] ciphertext)
        {
            Ciphertext = ciphertext;
            Iterations = iterations;
            InitializationVector = initializationVector;
            Salt = salt;
        }

        public byte[] Salt { get; private set; }
        
        public byte[] InitializationVector { get; private set; }
        
        public int Iterations { get; private set; }

        public byte[] Ciphertext { get; private set; }

    }
}
