using System.ComponentModel.DataAnnotations;

namespace QueryPerformanceMaster.Domain
{
    public enum ProfilerExecuterType
    {
        [Display(Name = "Paraller Executor")]
        ParallerExecutor,

        [Display(Name = "Sequential Executor")]
        SequentialExecutor,

        [Display(Name = "Sequential Executor With Delay")]
        SequentialExecutorWithDelay,

        [Display(Name = "Sequential Executor With Time Limit")]
        SequentialExecutorWithTimeLimit
    }
}