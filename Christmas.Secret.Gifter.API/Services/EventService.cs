﻿using AutoMapper;
using Christmas.Secret.Gifter.API.Services.Abstractions;
using Christmas.Secret.Gifter.Database.SQLite.Entries;
using Christmas.Secret.Gifter.Database.SQLite.Repositories.Abstractions;
using Christmas.Secret.Gifter.Domain;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Christmas.Secret.Gifter.API.Services
{
    public class EventService : IEventService
    {
        private readonly ILogger<EventService> _logger;
        private readonly IEventRepository _eventRepoistory;
        private readonly IMapper _mapper;

        public EventService(ILogger<EventService> logger, IEventRepository eventRepoistory, IMapper mapper)
        {
            _logger = logger;
            _eventRepoistory = eventRepoistory;
            _mapper = mapper;
        }

        public async Task<GiftEvent> AddAsync(GiftEvent item, CancellationToken cancellationToken)
        {
            var mapped = _mapper.Map<EventEntry>(item);
            var added = await _eventRepoistory.AddAsync(mapped, cancellationToken);

            return _mapper.Map<GiftEvent>(added);
        }

        public async Task<GiftEvent> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var found = await _eventRepoistory.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<GiftEvent>(found);
        }
    }
}
