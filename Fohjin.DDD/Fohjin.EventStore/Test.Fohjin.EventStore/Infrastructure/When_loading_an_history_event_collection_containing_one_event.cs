﻿using System.Linq;
using System.Collections.Generic;
using Fohjin.EventStore;
using Fohjin.EventStore.Configuration;
using Fohjin.EventStore.Infrastructure;

namespace Test.Fohjin.EventStore.Infrastructure
{
    public class When_loading_an_history_event_collection_containing_one_event : BaseTestFixture
    {
        private TestClient _testClient;

        protected override void Given()
        {
            var eventProcessorCache = PreProcessorHelper.CreateEventProcessorCache();
            _testClient = new DomainRepository(new AggregateRootFactory(eventProcessorCache, new ApprovedEntitiesCache())).CreateNew<TestClient>();
        }

        protected override void When()
        {
            var eventProvider = (IEventProvider)_testClient;
            eventProvider.LoadFromHistory(new List<IDomainEvent>{ new ClientMovedEvent(new Address("street", "number", "postalCode", "city"))});
        }

        [Then]
        public void Then_the_internal_collection_publiched_of_events_will_be_empty()
        {
            var eventProvider = (IEventProvider) _testClient;
            eventProvider.GetChanges().Count().WillBe(0);
        }

        [Then]
        public void Then_the_state_of_the_entity_is_updated()
        {
            _testClient.GetAddress().Street.WillBe("street");
            _testClient.GetAddress().Number.WillBe("number");
            _testClient.GetAddress().PostalCode.WillBe("postalCode");
            _testClient.GetAddress().City.WillBe("city");
        }
    }
}