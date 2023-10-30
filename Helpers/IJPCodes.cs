using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IJPReporting.Helpers
{
    public class IJPCodes
    {
        public enum QuestionType
        {
            RadioButtonList = 1,
            DropdownList = 2,
            TextBox = 3,
            DatePicker = 4,
            CheckBoxList = 5,
            MultipleDropdownList = 6,
            TextArea = 12
        }

        public enum Condition
        {
            Hide = 3,
            Show = 4,
            EnableResponseChoice = 6,
            Finish = 5,
            ChangeAcceptanceStatus = 7,
            ChangeCompletionStatus = 8
        }

        public enum Operator
        {
            Equal = 1
        }

        public enum FormStatusCode
        {
            Accepted = 1,
            NotAccepted = 2,
            PendingInformation = 3,
            Completed = 4,
            NotCompleted = 5,
            Ongoing = 9
        }

    }

    static class FormStatus
    {
        public const string Accepted = "accept";
        public const string NotAccepted = "not_accept";
        public const string PendingInformation = "pend";
        public const string Completed = "comp";
        public const string NotCompleted = "not_comp";
        public const string Ongoing = "ongo";
    }

    static class RolesCode
    {
        public const string RecipientUmbOrg = "7ede42e5-ee02-445c-8dc4-3ff620fa68b5";
        public const string RecipientPT = "b196887b-ab7c-4cc9-8f54-a7bf855883a9";
        public const string IJPRegionalCoordinator = "f6b3a970-18c4-4b32-bf78-4336d234bbfd";
        public const string Program = "4353f173-0ce5-4cec-8327-79bc9c3415c9";
        public const string IJPReadAll = "96ba5bfd-914f-4acb-90b8-05ab2831eaf2";
        public const string PTRead = "08c13181-1d64-4767-966e-f408f8b30422";
        public const string Admin = "d3dedfba-6620-4603-b95c-ad4d77571941";
    }
}