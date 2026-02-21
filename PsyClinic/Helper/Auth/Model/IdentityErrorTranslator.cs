public static class IdentityErrorTranslator
{
    public static string Translate(string code)
    {
        return code switch
        {
            "PasswordTooShort" => "A senha deve ter pelo menos 8 caracteres.",
            "PasswordRequiresDigit" => "A senha deve conter pelo menos um número.",
            "PasswordRequiresLower" => "A senha deve conter pelo menos uma letra minúscula.",
            "PasswordRequiresUpper" => "A senha deve conter pelo menos uma letra maiúscula.",
            "PasswordRequiresNonAlphanumeric" => "A senha deve conter pelo menos um caractere especial.",
            "DuplicateUserName" => "Já existe um usuário com esse e-mail.",
            "DuplicateEmail" => "Já existe um usuário com esse e-mail.",
            "InvalidEmail" => "O e-mail informado é inválido.",
            _ => "Erro ao criar usuário. Verifique os dados informados."
        };
    }
}