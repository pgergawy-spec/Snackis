using SnackisAPI.Exceptions;

namespace SnackisAPI.Helpers;

public static class AccessRules
{
    public static void EnsureOwnerOrAdmin(string ownerUserId, string currentUserId, bool isAdmin, string errorMessage)
    {
        var isOwner = ownerUserId == currentUserId;

        if (!isOwner && !isAdmin)
            throw new ForbiddenException(errorMessage);
    }
}
