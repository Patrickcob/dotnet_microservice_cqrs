using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.CMD.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active
        {
            get => _active; set => _active = value;
        }

        public PostAggregate()
        {

        }

        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.Now

            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;
        }

        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit the message of an inactive post!");
            }

            if (string.IsNullOrEmpty(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty. Please provide valid {nameof(message)}");
            }

            RaiseEvent(new MessageUpdatedEvent {
                Id = _id,
                Message = message
            });
            
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.Id;
        }

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like an inactive post");
            }

            RaiseEvent(new PostLikedEvent {
                Id = _id
            });
        }

        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;
        }

        public void AddComment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot add a comment an inactive post");
            }

            if (string.IsNullOrEmpty(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide valid {nameof(comment)}");
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new InvalidOperationException($"The value of {nameof(username)} cannot be null or empty. Please provide valid {nameof(username)}");
            }

            RaiseEvent(new CommentAddedEvent 
            {
                Id = _id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = username,
                CommentDate = DateTime.Now
            });

        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }

        public void EditComment(Guid commentId, string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit a comment an inactive post");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("Not allowed to edit comment from other user");
            }

            RaiseEvent(new CommentUpdateEvent 
            {
                Id = _id,
                CommentId = commentId,
                Comment = comment,
                Username = username,
                EditDate = DateTime.Now
            });

        }

        public void Apply(CommentUpdateEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        public void RemoveComment(Guid commentId, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot remove a comment an inactive post");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("Not allowed to remove comment from other user");
            }

            RaiseEvent(new CommentRemovedEvent 
            {
                Id = _id,
                CommentId = commentId
            });

        }

        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);

        }

        public void DeletePost(string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot remove a comment an inactive post");
            }

            if (!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You cannot remove a comment from someone else");               
            }

            RaiseEvent(new PostRemovedEvent 
            {
                Id = _id
            });
            
        }

        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }

    }
}