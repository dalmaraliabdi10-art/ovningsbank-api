namespace OvningsbankApi.Models;

/// <summary>
/// Var övningen befinner sig i tränarens planering.
/// Hålls skild från svårighetsgrad: svårighetsgrad är en egenskap hos övningen
/// och ändras sällan, medan status ändras varje gång ett pass genomförts.
/// Separationen ger PUT-endpointen ett meningsfullt användningsfall.
/// </summary>
public enum ExerciseStatus
{
    Planerad,
    Genomford
}
