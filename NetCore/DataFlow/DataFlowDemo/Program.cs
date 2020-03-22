using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DataFlowDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var processedDelaysCollection = new BlockingCollection<int>();
            var options = new ExecutionDataflowBlockOptions{
                MaxDegreeOfParallelism = 4
            };

            var actionBlock = new ActionBlock<int>(async delay =>
            {
                if (delay == 3 || delay == 5)
                {
                    throw new ArgumentException($"Nope delay {delay} is not supported, not at all");
                }

                await Task.Delay(1000 * delay);

                processedDelaysCollection.Add(delay);
            }, options);

            var stopWatch = Stopwatch.StartNew();
            foreach(var delay in new int[] {8, 6, 5, 3 })
            {
                actionBlock.Post(delay);
            }

            actionBlock.Complete();
            try 
            {
                await actionBlock.Completion;
            }
            catch (AggregateException errors)
            {
                foreach (var ex in errors.InnerExceptions)
                    Console.WriteLine($"{ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            stopWatch.Stop();
            Console.WriteLine($"Processing took {stopWatch.Elapsed} secs");
            Console.WriteLine($"Processed delays : {string.Join(',', processedDelaysCollection)}");
        }
    }
}
