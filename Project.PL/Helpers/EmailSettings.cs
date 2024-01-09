using Project.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Project.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var clinet = new SmtpClient("smtp.gmail.com", 587);
			clinet.EnableSsl = true;
			clinet.Credentials = new NetworkCredential("abdelrahmanzaky300@gmail.com", "bauqdjhesfnxgkfg");
			clinet.Send("abdelrahmanzaky300@gmail.com", email.Recipients, email.Subject, email.Body);

		}
	}
}
