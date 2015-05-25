namespace MinaGlosor.Web.Infrastructure.Tracing
{
    // ReSharper disable InconsistentNaming
    public static class EventIds
    {
        /// <summary>
        /// Meaning = Positive Occasional Preliminary.
        /// Example = Service starting.
        /// Event Type = Information.
        /// </summary>
        public static class Informational_Preliminary_1XXX
        {
            // Web starts at 1000
            public const int Web_Starting_1000 = 1000;

            public const int Web_Request_Executing_1001 = 1001;

            // Notification service starts at 1100
        }

        /// <summary>
        /// Meaning = Positive Occasional Completion.
        /// Example = Connection made, or user logged on.
        /// Event Type = Information.
        /// </summary>
        public static class Informational_Completion_2XXX
        {
            // Web starts at 2000
            public const int Web_Started_2000 = 2000;

            public const int Web_Request_Executed_2001 = 2001;

            // Notification service starts at 2100
        }

        /// <summary>
        /// Meaning = Positive Frequent Intermediate.
        /// Example = Command received.
        /// Event Type = Informational and application log.
        /// </summary>
        public static class Informational_ApplicationLog_3XXX
        {
            // Web starts at 3000
            public const int Web_ExecuteCommand_3000 = 3000;

            public const int Web_ExecuteDependenCommand_3001 = 3001;

            public const int Web_RaiseEvent_3002 = 3002;

            // Notification service starts at 3100
        }

        /// <summary>
        /// Meaning = Transient Negative.
        /// Example = Warning.
        /// Event Type = Warning.
        /// </summary>
        public static class Warning_Transient_4XXX
        {
            // Web starts at 4000
            public const int Web_Message_Timeout_4000 = 4000;

            public const int Web_PracticeSession_Finished_4001 = 4001;

            public const int Web_ChangesFromQuery_4002 = 4002;

            // Notification service starts at 4100
        }

        /// <summary>
        /// Meaning = Permanent Negative.
        /// Example = Error.
        /// Event Type = Error.
        /// </summary>
        public static class Error_Permanent_5XXX
        {
            // Web starts at 5000
            public const int Web_WrongConfiguration_5000 = 5000;

            public const int Web_CreatePracticeSession_Unauthorized_5001 = 5001;

            public const int Web_GetNextPracticeWord_Unauthorized_5002 = 5002;

            // Notification service starts at 5100
        }

        /// <summary>
        /// Meaning = Positive Occasional Finalization.
        /// Example = Service stopped.
        /// Event Type = Information.
        /// </summary>
        public static class Information_Finalization_8XXX
        {
            // Web starts at 8000
            public const int Web_Stopping_8000 = 8000;

            public const int Web_Stopped_8001 = 8001;

            // Notification service starts at 8100
        }

        /// <summary>
        /// Meaning = Unknown (Error, forced to exit).
        /// Example = Unhandled exception.
        /// Event Type = Error (Critical).
        /// </summary>
        public static class Critical_Unknown_9XXX
        {
            // Web starts at 9000
            public const int Web_Unknown_9000 = 9000;

            // Notification service starts at 9100
        }
    }
}