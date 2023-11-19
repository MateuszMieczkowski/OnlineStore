using OnlineStore.Server.Entities;

namespace OnlineStore.Server.Emails.EmailDefinitions
{
    public class ResetPasswordEmail : EmailDefinition
    {
        private readonly string _resetLink;
        public ResetPasswordEmail(string resetLink, string recipientEmail, string? recipientName, string? senderEmail) : base(recipientEmail, recipientName, senderEmail)
        {
            _resetLink = resetLink;
        }

        public override string Subject => "Resetowanie hasła";
        public override string TemplateName => "ResetPassword";
        public override ICollection<EmailReplacement> GetReplacements()
        {
          var replacements = new List<EmailReplacement>
          { 
              new("{{ResetLink}}", _resetLink),
              new ("{{UserEmail}}", RecipientEmail),
          };

          return replacements;
        }
    }
}
