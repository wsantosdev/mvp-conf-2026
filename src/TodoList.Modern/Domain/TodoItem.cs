namespace TodoList.Modern.Domain;

public class TodoItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsCancelled { get; private set; }

    public TodoItem(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("O título da tarefa não pode ser vazio.", nameof(title));

        Id = Guid.NewGuid();
        Title = title;
        IsCompleted = false;
        IsCancelled = false;
    }

    public void Cancel()
    {
        if (IsCompleted)
            throw new InvalidOperationException("Não é possível cancelar uma tarefa já concluída.");

        IsCancelled = true;
    }

    public void Complete()
    {
        if (IsCancelled)
            throw new InvalidOperationException("Não é possível concluir uma tarefa já cancelada.");

        IsCompleted = true;
    }
}
