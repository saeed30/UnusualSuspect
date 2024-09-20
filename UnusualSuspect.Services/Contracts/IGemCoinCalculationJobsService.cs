namespace UnusualSuspect.Services.Contracts;

public interface IGemCoinCalculationJobsService
{
  Task RecalculateAllUsersGemAndCoin();
  Task RecalculateGemAndCoinByUserId(int userId);
  Task ValidatePayments(int userId);
}