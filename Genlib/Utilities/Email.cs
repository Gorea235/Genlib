using System;
using System.Collections.Generic;
using System.Text;

namespace Genlib.Utilities
{
    public class Email
    {
        private enum EmailProcessState
        {
            LocalPart,
            LocalPostPeriod,
            LocalInQuote,
            LocalPostQuote,
            DoaminPart,
            DomainPostHyphen,
            DomainPostPeriod
        }

        private string m_local;
        private string m_domain;

        private readonly char[] m_charsAllowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
        private readonly char[] m_charsSpecial = "!#$%&'*+-/=?^_`{|}~;".ToCharArray();
        private readonly char[] m_charsQuoteOnly = @"(),:;<>@[]".ToCharArray();
        private readonly char[] m_charsEscQuoteOnly = @"""\".ToCharArray();

        public string LocalPart { get { return m_local; } }
        public string DomainPart { get { return m_domain; } }
        public string FullAddress { get { return LocalPart + "@" + DomainPart; } }

        public Email(string address) : this(address, EmailOptions.None) { }

        public Email(string address, EmailOptions options)
        {
            if (address == null)
                throw new ArgumentNullException();

            EmailProcessState cstate = EmailProcessState.LocalPart;
            StringBuilder clocal = new StringBuilder();
            StringBuilder cdomain = new StringBuilder();
            foreach (char chr in address)
            {
                switch (cstate)
                {
                    case EmailProcessState.LocalPart:
                        if (chr == '"')
                        {

                        }
                        else if (chr == '.')
                        {

                        }
                        else if (Array.Exists(m_charsAllowed, (e) => e == chr))
                        {

                        }
                        else if (Array.Exists(m_charsSpecial, (e) => e == chr))
                        {

                        }
                        else
                            throw new EmailFormatException(string.Format(
                                "Character '{0}' is not allowed in local part of an email address.", chr));
                        break;
                    case EmailProcessState.LocalPostPeriod:

                        break;
                    case EmailProcessState.LocalInQuote:

                        break;
                    case EmailProcessState.LocalPostQuote:

                        break;
                    case EmailProcessState.DoaminPart:

                        break;
                    case EmailProcessState.DomainPostHyphen:

                        break;
                    case EmailProcessState.DomainPostPeriod:

                        break;
                }
            }
            if (string.IsNullOrEmpty(m_local) ||
                string.IsNullOrEmpty(m_domain))
                throw new EmailFormatException("Local part or domain contained no characters.");
            m_local = clocal.ToString();
            m_domain = cdomain.ToString();
        }
    }
}
