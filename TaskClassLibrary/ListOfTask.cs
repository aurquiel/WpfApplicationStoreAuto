using System.Collections.Concurrent;

namespace TaskClassLibrary
{
    public class ListOfTask
    {
        public static ConcurrentQueue<Tasks> TasksQueue = new ConcurrentQueue<Tasks>();

        public static bool EmptyQueue()
        {
            Tasks task = new Tasks();
            while(TasksQueue.Count > 0)
            {
                if(TasksQueue.TryDequeue(out task) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
