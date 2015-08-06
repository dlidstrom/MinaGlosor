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

            public const int Web_Register_TaskRunner_1002 = 1002;
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

            public const int Web_ExecuteDependentCommand_3001 = 3001;

            public const int Web_RaiseEvent_3002 = 3002;

            public const int Web_ScoreNotUpdated_3003 = 3003;

            public const int Web_CreateWebsiteConfig_3004 = 3004;

            public const int Web_CheckVersion_3005 = 3005;

            public const int Web_ExecuteAdminCommand_3006 = 3006;

            public const int Web_StartTask_3007 = 3007;

            public const int Web_EndTask_3008 = 3008;

            public const int Web_SendTask_3009 = 3009;

            public const int Web_ExecuteCommandStart_3010 = 3010;

            public const int Web_ExecuteCommandStop_3011 = 3011;
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

            public const int Web_TaskInProcess_4003 = 4003;

            public const int Web_TaskRunner_TimeOut_4004 = 4004;
        }

        /// <summary>
        /// Meaning = Permanent Negative.
        /// Example = Error.
        /// Event Type = Error.
        /// </summary>
        public static class Error_Permanent_5XXX
        {
            // Web starts at 5000
            public const int Web_UnhandledException_5000 = 5000;

            public const int Web_CreatePracticeSession_Unauthorized_5001 = 5001;

            public const int Web_GetNextPracticeWord_Unauthorized_5002 = 5002;

            public const int Web_MissingApplyEvent_5003 = 5003;
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

            public const int Web_Unregister_TaskRunner_8002 = 8002;
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