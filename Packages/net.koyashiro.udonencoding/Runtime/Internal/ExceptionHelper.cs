using UnityEngine;

namespace Koyashiro.UdonEncoding.Internal
{
    public static class ExceptionHelper
    {
        private const string TAG = "UdonEncoding";
        private const string COLOR_TAG = "red";
        private const string COLOR_EXCEPTION = "lime";
        private const string COLOR_PARAMETER = "cyan";
        private const string COLOR_ACTUAL_VALUE = "magenta";

        public static void ThrowArgumentException(string message)
        {
            LogErrorMessage(typeof(System.ArgumentException).FullName, message);
            Panic();
        }

        public static void ThrowArgumentNullException(string paramName)
        {
            LogErrorMessage(typeof(System.ArgumentNullException).FullName, "Value cannot be null.", paramName);
            Panic();
        }

        private static void LogErrorMessage(string exception, string message)
        {
            Debug.LogError($"[<color={COLOR_TAG}>{TAG}</color>] <color={COLOR_EXCEPTION}>{exception}</color>: {message}");
        }

        private static void LogErrorMessage(string exception, string message, string paramName)
        {
            Debug.LogError($"[<color={COLOR_TAG}>{TAG}</color>] <color={COLOR_EXCEPTION}>{exception}</color>: {message} (Parameter '<color={COLOR_PARAMETER}>{paramName}</color>')");
        }

        private static void Panic()
        {
            // Raise runtime Exception
            ((object)null).ToString();
        }
    }
}
