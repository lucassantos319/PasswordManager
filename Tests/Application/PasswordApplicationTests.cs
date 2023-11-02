using Application.Services;
using Infra.Data;

namespace Tests.Application
{
    public class PasswordApplicationTests
    {
        public static string key = "b14ca5898a4e4133bbce2ea2315a1916";
        private static string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=root;Database=Passwords";
        private PasswordApplication _service;

        [Test]
        [TestCase("teste", "caixa", 1)]
        [TestCase("TESTE2", "opt", 1)]
        [TestCase("TES12E2", "opt", 1)]
        public void AddPasswordTest(string password, string name, int typeId)
        {
            try
            {
                _service.AddPassword(password, name, typeId);
                var passwords = _service.GetPasswordsFromType(typeId);

                var dbPassword = passwords.FirstOrDefault(x => x.Name == name);

                if (dbPassword == null)
                    Assert.Fail("Not insert");
            }
            catch (Exception ex)
            {
                if (!ex.Message.Equals("Senha já existe"))
                    Assert.Fail(ex.Message);
            }
        }

        [Test]
        [TestCase(1, 11)]
        public void Decrypted(int typeId, int passwordId)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [SetUp]
        public void Setup()
        {
            _service = new PasswordApplication(_connectionString);
        }
    }
}