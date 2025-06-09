using MediatR;
using Online_Health_Consultation_Portal.Application.Queries.Users;
using Online_Health_Consultation_Portal.Domain;
using Online_Health_Consultation_Portal.Infrastructure.Repositories;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Application.Handlers.Users
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IRepository<User> _userRepository;

        public GetUserByIdQueryHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            return user; // trả thẳng entity User luôn
        }
    }
}
