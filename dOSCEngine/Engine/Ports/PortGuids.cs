namespace dOSCEngine.Engine.Ports
{
    public static class PortGuids
    {
        public static readonly Guid Port_1 = new Guid("00000000-0000-0000-0000-000000000001");
        public static readonly Guid Port_2 = new Guid("00000000-0000-0000-0000-000000000002");
        public static readonly Guid Port_3 = new Guid("00000000-0000-0000-0000-000000000003");
        public static readonly Guid Port_4 = new Guid("00000000-0000-0000-0000-000000000004");
        public static readonly Guid Port_5 = new Guid("00000000-0000-0000-0000-000000000005");
        public static readonly Guid Port_6 = new Guid("00000000-0000-0000-0000-000000000006");
        public static readonly Guid Port_7 = new Guid("00000000-0000-0000-0000-000000000007");
        public static readonly Guid Port_8 = new Guid("00000000-0000-0000-0000-000000000008");
        public static readonly Guid Port_9 = new Guid("00000000-0000-0000-0000-000000000009");
        public static readonly Guid Port_10 = new Guid("00000000-0000-0000-0000-000000000010");
        public static readonly Guid Port_11 = new Guid("00000000-0000-0000-0000-000000000011");
        public static readonly Guid Port_12 = new Guid("00000000-0000-0000-0000-000000000012");
        public static readonly Guid Port_13 = new Guid("00000000-0000-0000-0000-000000000013");
        public static readonly Guid Port_14 = new Guid("00000000-0000-0000-0000-000000000014");
        public static readonly Guid Port_15 = new Guid("00000000-0000-0000-0000-000000000015");
        public static readonly Guid Port_16 = new Guid("00000000-0000-0000-0000-000000000016");
        public static Guid PortGuidGenerator(int portNumber = 0)
        {
            return new Guid("00000000-0000-0000-0000-" + portNumber.ToString("000000000000"));
        }
    }
}
