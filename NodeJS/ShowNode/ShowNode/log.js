var bunyan = require('bunyan');
// Imports the Google Cloud client library for Bunyan
var LoggingBunyan = require('@google-cloud/logging-bunyan');
function writeLog() {
    // Instantiates a Bunyan Stackdriver Logging client
    var loggingBunyan = LoggingBunyan();
    // Create a Bunyan logger that streams to Stackdriver Logging
    // Logs will be written to: "projects/YOUR_PROJECT_ID/logs/bunyan_log"
    var logger = bunyan.createLogger({
        // The JSON payload of the log as it appears in Stackdriver Logging
        // will contain "name": "my-service"
        name: 'my-service',
        // log at 'info' and above
        level: 'info',
        streams: [
            // Log to the console
            { stream: process.stdout },
            // And log to Stackdriver Logging
            loggingBunyan.stream()
        ]
    });
    // Writes some log entries
    logger.error('warp nacelles offline');
    logger.info('shields at 99%');
}
writeLog();
//# sourceMappingURL=log.js.map