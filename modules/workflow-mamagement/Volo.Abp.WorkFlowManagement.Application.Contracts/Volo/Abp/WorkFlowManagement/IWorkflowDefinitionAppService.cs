﻿using Elsa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.WorkFlowManagement
{
    public interface IWorkflowDefinitionAppService : IApplicationService
    {
        Task CreateAsync(WorkflowDefinitionCreateDto input);
        
        Task<List<WorkflowDefinitionListDto>> GetListAsync();

        Task DeleteAsync(string id);
    }
}
