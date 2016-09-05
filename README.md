# CheckRepublic

Check on things to make sure they're working. A monitoring web service. 

## Introduction

CheckRepublic is a web service that can run arbitrary "checks" on other resources. For example, I have a check that
ensures that my blog is up and another to ensure that one of my background jobs has sent a heartbeat recently.

All of the check results, heartbeats, and notifications are stored in a SQLite database.

Two endpoints should be regularly hit with an HTTP POST:

1. `/api/checkrunner`: runs all of the checks and records the results to the database.
1. `/api/notificationrunner`: sends out notifications if any of the checks have been failing for too long.

The following "read" endpoints are notable:

1. `/api/checkbatches/latest`: the results of the latest checks.
1. `/api/health/runner`: returns HTTP 500 if the checks have not been run recently (should be hit by an external monitoring service).
