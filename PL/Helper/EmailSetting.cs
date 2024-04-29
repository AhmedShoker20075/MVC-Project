using DAL.Models;
using System.Net;
using System.Net.Mail;

namespace PL.Helper
{
	public class EmailSetting
	{
		public static void SendEmail(Email email)
		{
			//Mail Server : gmail
			var Client = new SmtpClient("smtp.gmail.com",587);
			Client.EnableSsl = true;
			Client.Credentials = new NetworkCredential("ahmed.sayed20075@gmail.com", "fayp iybs tcrr jxbi");
			Client.Send("ahmed.sayed20075@gmail.com",email.Reciept,email.Subject,email.Body);
		}
	}
}
