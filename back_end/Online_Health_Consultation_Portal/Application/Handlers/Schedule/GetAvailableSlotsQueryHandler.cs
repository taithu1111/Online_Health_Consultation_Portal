using MediatR;
using Microsoft.EntityFrameworkCore;
using Online_Health_Consultation_Portal.Application.Dtos.Schedule;
using Online_Health_Consultation_Portal.Application.Queries.Schedule;
using Online_Health_Consultation_Portal.Infrastructure;
using Online_Health_Consultation_Portal.Infrastructure.Repository;

namespace Online_Health_Consultation_Portal.Application.Handlers.Schedule
{
    public class GetAvailableSlotsQueryHandler : IRequestHandler<GetAvailableSlotsQuery, List<AvailableSlotDto>>
    {
        private readonly AppDbContext _context;
        private readonly IAutoMapper _mapper;
        public GetAvailableSlotsQueryHandler(AppDbContext context, IAutoMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<AvailableSlotDto>> Handle(GetAvailableSlotsQuery request, CancellationToken cancellationToken)
        {
            var date = request.Date.Date;

            // Lấy ca làm việc trong ngày
            var schedules = await _context.Schedules
                .Where(s =>
                    s.DoctorId == request.DoctorId &&
                    //s.Date == date.Date &&
                    //s.IsAvailable)
                    s.Date >= date.Date &&
                    s.Date < date.Date.AddDays(1) &&
                    s.IsAvailable)
                .ToListAsync(cancellationToken);

            var slots = new List<AvailableSlotDto>();

            foreach (var sched in schedules)
            {
                var current = date + sched.StartTime;
                var endTime = date + sched.EndTime;

                // Tính độ dài mỗi slot (mặc định 30 phút)
                var slotDuration = TimeSpan.FromMinutes(
                    (endTime - current).TotalMinutes /
                    Math.Ceiling((endTime - current).TotalMinutes / 30)
                );

                // Sinh từng AvailableSlotDto
                while (current + slotDuration <= endTime)
                {
                    slots.Add(new AvailableSlotDto
                    {
                        SlotStart = current,
                        SlotEnd = current + slotDuration
                    });
                    current += slotDuration;
                }
            }

            // Trả luôn list DTO
            return slots;

            //------------------------------------------
            // sử dụng mapper
            //var date = request.Date.Date;
            //var schedules = await _context.Schedules
            //    .Where(s => s.DoctorId == request.DoctorId && s.DayOfWeek == date.DayOfWeek && s.IsAvailable)
            //    .ToListAsync(cancellationToken);

            //var slot = new List<AvailableSlotDto>();
            //// lấy danh sách các cuộc hẹn đã đặt trong ngày
            //foreach (var schedule in schedules)
            //{
            //    var startTime = date + schedule.StartTime;
            //    var endTime = date + schedule.EndTime;
            //    // tính thời gian bắt đầu và kết thúc của các slot
            //    var SlotDuration = TimeSpan.FromMinutes(
            //        (endTime - startTime).TotalMinutes / 
            //        Math.Ceiling((endTime - startTime).TotalMinutes / 30));// thời gian của mỗi slot là 30 phút

            //    while (startTime + SlotDuration <= endTime)
            //    {
            //        // kiểm tra xem slot có bị trùng với các cuộc hẹn đã đặt không
            //        slot.Add(new AvailableSlotDto
            //        {
            //            SlotStart = startTime,
            //            SlotEnd = startTime + SlotDuration
            //        });
            //        startTime += SlotDuration; // tăng thời gian bắt đầu của slot lên theo SlotDuration
            //    }
            //}
            //return _mapper.Map<List<AvailableSlotDto>>(slot);

        }
    }
}
