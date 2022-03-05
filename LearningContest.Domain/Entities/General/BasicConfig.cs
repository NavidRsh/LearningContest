using CommonInfrastructure.Persistence.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using CommonInfrastructure.General.Enums;
using CommonInfrastructure.Attribute;

namespace LearningContest.Domain.Entities.General
{
    public class BasicConfig
    {
        [EntityConfigAttribute(IsKey = true, Update = ConfigItemState.Exist, Dto = ConfigItemState.Exist)]
        public int Id { get; set; }
        [EntityConfigAttribute(Insert = ConfigItemState.Exist, Update = ConfigItemState.Exist, Dto = ConfigItemState.Exist)]
        public CommonInfrastructure.General.Enums.BasicConfigKeyEnum ItemKey { get; set; }
        [EntityConfigAttribute(Insert = ConfigItemState.Exist, Update = ConfigItemState.Exist, Dto = ConfigItemState.Exist)]
        public string ItemValue { get; set; }
        [EntityConfigAttribute(Insert = ConfigItemState.Exist, Update = ConfigItemState.Exist, Dto = ConfigItemState.Exist)]
        public string Description { get; set; }
        [EntityConfigAttribute(Insert = ConfigItemState.Exist, Update = ConfigItemState.Exist, Dto = ConfigItemState.Exist)]
        public ModuleEnum Module { get; set; }


    }
    
}
