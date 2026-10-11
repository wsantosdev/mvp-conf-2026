namespace TodoList.Modern.Domain;

public sealed class TodoItem(string title)
{
    public string Title { get; private set; } = ValidateTitle(title);
    public Guid Id { get; private set; } = Guid.NewGuid();
    public bool IsCompleted { get; private set; }
    public bool IsCancelled { get; private set; }

    private static string ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("O título da tarefa não pode ser vazio.", nameof(title));

        return title;
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
