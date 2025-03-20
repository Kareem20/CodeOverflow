namespace CodeOverFlow.Data
{
    public static class DbMetaData
    {
        // User Table
        public const string USER_TABLE = "[dbo].[User]";
        public const string USER_ID_COLUMN = "UserID";
        public const string USER_FIRST_NAME_COLUMN = "first_name";
        public const string USER_LAST_NAME_COLUMN = "last_name";
        public const string USER_EMAIL_COLUMN = "email";
        public const string USER_USERNAME_COLUMN = "username";
        public const string USER_PASSWORD_COLUMN = "password";

        // Tag Table
        public const string TAG_TABLE = "Tag";
        public const string TAG_ID_COLUMN = "TagID";
        public const string TAG_NAME_COLUMN = "name";

        // QuestionTag Table
        public const string QUESTION_TAG_TABLE = "QuestionTag";

        // UserTag Table
        public const string USER_TAG_TABLE = "UserTag";

        // Question Table
        public const string QUESTION_TABLE = "Question";
        public const string QUESTION_ID_COLUMN = "QuestionID";
        public const string QUESTION_TITLE_COLUMN = "title";
        public const string QUESTION_TEXT_COLUMN = "text";
        public const string QUESTION_TIMESTAMP_COLUMN = "timestamp";

        // Answer Table
        public const string ANSWER_TABLE = "Answer";
        public const string ANSWER_ID_COLUMN = "AnswerID";
        public const string ANSWER_TEXT_COLUMN = "text";
        public const string ANSWER_TIMESTAMP_COLUMN = "timestamp";
        public const string ANSWER_ISEDITED_COLUMN = "isEdited";

        // Vote Table
        public const string VOTE_TABLE = "Vote";
        public const string VOTE_ID_COLUMN = "VoteID";
        public const string VOTE_TYPE_COLUMN = "VoteType";
        public const string VOTE_TIMESTAMP_COLUMN = "timestamp";


    }
}
