using ClassLib;

namespace JVM
{
    internal class Program
    {       
        static void Main(string[] args)
        {
            byte[] MachineCode = File.ReadAllBytes(@"\\GMRDC1\Folder Redirection\rafael.sternberg\Documents\JavaTesting\JavaTest.class");

            ClassFile classFile = new ClassFile(MachineCode);
        }
    }
}
