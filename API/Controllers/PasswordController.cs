using API.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PasswordController : ControllerBase
    {
        public PasswordController()
        {
            var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=root;Database=Passwords";

            if (_application == null)
            {
                _application = new PasswordApplication(connectionString);
            }
        }

        private PasswordApplication _application { get; set; }

        [HttpPost]
        public ActionResult AddPassword([FromBody] PasswordObj password)
        {
            try
            {
                var id = _application.AddPassword(password.password, password.name, 1);
                return Ok(new RequestResponse
                {
                    msg = id.ToString()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new RequestResponse
                {
                    msg = ex.Message
                });
            }
        }

        [HttpDelete]
        public ActionResult DeletePassword([FromQuery] int passwordId)
        {
            try
            {
                _application.DeletePassword(passwordId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new RequestResponse
                {
                    msg = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("password")]
        public RequestResponse GetPassword([FromQuery] int passwordId)
        {
            try
            {
                var password = _application.DecryptPassword(1, passwordId);

                return new RequestResponse
                {
                    msg = password
                };
            }
            catch (Exception ex)
            {
                return new RequestResponse
                {
                    msg = ex.Message
                };
            }
        }

        [HttpGet]
        public RequestResponse GetPasswords()
        {
            try
            {
                var passwords = _application.GetPasswordsFromType(1);
                var bodyResponse = JsonConvert.SerializeObject(passwords);

                if (bodyResponse != null)
                    return new RequestResponse
                    {
                        msg = bodyResponse
                    };

                return new RequestResponse
                {
                    msg = "Não encontrado nenhuma senha !"
                };
            }
            catch (Exception ex)
            {
                return new RequestResponse
                {
                    msg = ex.Message
                };
            }
        }
    }

    public class PasswordObj
    {
        public string name { get; set; }
        public string password { get; set; }
    }
}