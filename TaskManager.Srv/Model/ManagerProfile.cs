using AutoMapper;

using TaskManager.Srv.Model.DataModel;
using TaskManager.Srv.Model.ViewModel;

namespace TaskManager.Srv.Model;

public class ManagerProfile : Profile
{
    public ManagerProfile()
    {
        CreateMap<Project, ProjectViewModel>().ReverseMap();
        CreateMap<User, UserViewModel>().ReverseMap();
        CreateMap<WiLinkTemplate, WiLinkTemplateViewModel>().ReverseMap();
        CreateMap<ProjectTask, TaskViewModel>().ReverseMap();
        CreateMap<CommentLine, CommentViewModel>().ReverseMap();
        CreateMap<ConnectingWiDb, WiViewModel>().ReverseMap();
        CreateMap<TaskMilestone, MilestoneViewModel>().ReverseMap();
    }
}