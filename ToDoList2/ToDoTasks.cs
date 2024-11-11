public class ToDoTask
{
    public string Description { get; set; }
    public bool IsCompleted { get; set; }

    public ToDoTask(string description)
    {
        Description = description;
        IsCompleted = false;
    }
}