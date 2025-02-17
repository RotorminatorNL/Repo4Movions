﻿using Application.AdminModels;
using Domain.Enums;
using System;

namespace Application.Validation
{
    public class CrewMemberValidation
    {
        public bool IsInputValid(AdminCrewMemberModel adminCrewMemberModel)
        {
            bool isCharacterNameOk = adminCrewMemberModel.CharacterName == null;
            bool isRoleOk = Enum.IsDefined(typeof(CrewRoles), adminCrewMemberModel.Role);

            if (adminCrewMemberModel.Role == CrewRoles.Actor)
            {
                isCharacterNameOk = !(adminCrewMemberModel.CharacterName == null || adminCrewMemberModel.CharacterName == "");
            }

            return isCharacterNameOk && isRoleOk;
        }
    }
}
