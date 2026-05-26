namespace FIAP.AquaGuard.Exception.ExceptionsBase;

public class ErrorOnValidationException(string message) : AquaguardException(message)
{
}
