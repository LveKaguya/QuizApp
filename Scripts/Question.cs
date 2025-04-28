using System;

[Serializable]
public class Question
{
    public string text;
    public string[] options;
    public int correctIndex;
    public QuestionType type;

    public Question(string text, string[] options, int correctIndex, QuestionType type)
    {
        this.text = text;
        this.options = options;
        this.correctIndex = correctIndex;
        this.type = type;
    }
}

public enum QuestionType
{
    MultipleChoice,
    TrueFalse,
    WordGame
}