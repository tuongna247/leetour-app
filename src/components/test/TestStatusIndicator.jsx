import React, { useState, useEffect } from 'react';
import {
  Box,
  Card,
  CardContent,
  Typography,
  IconButton,
  Chip,
  LinearProgress,
  Collapse,
  Alert,
  Stack,
  Button,
  Tooltip
} from '@mui/material';
import {
  PlayArrow as PlayIcon,
  Pause as PauseIcon,
  Stop as StopIcon,
  Refresh as RefreshIcon,
  ExpandMore as ExpandMoreIcon,
  ExpandLess as ExpandLessIcon,
  CheckCircle as SuccessIcon,
  Error as ErrorIcon,
  Warning as WarningIcon,
  Info as InfoIcon
} from '@mui/icons-material';
import ErrorDisplay from '../ui/ErrorDisplay';

const TestStatusIndicator = ({ 
  testName = "Booking Test",
  status = "idle", // idle, running, success, error, warning
  progress = 0,
  message = "",
  error = null,
  onStart = null,
  onStop = null,
  onRetry = null,
  showControls = true,
  autoHide = false,
  hideDelay = 5000,
  maxRetries = 3,
  retryDelay = 1000,
  autoRetry = false
}) => {
  const [expanded, setExpanded] = useState(false);
  const [visible, setVisible] = useState(true);
  const [retryCount, setRetryCount] = useState(0);
  const [isRetrying, setIsRetrying] = useState(false);
  const [retryTimer, setRetryTimer] = useState(null);

  useEffect(() => {
    if (autoHide && (status === 'success' || status === 'error')) {
      const timer = setTimeout(() => {
        setVisible(false);
      }, hideDelay);
      
      return () => clearTimeout(timer);
    }
  }, [status, autoHide, hideDelay]);

  // Auto-retry logic
  useEffect(() => {
    if (autoRetry && status === 'error' && retryCount < maxRetries && onRetry) {
      setIsRetrying(true);
      const timer = setTimeout(() => {
        setRetryCount(prev => prev + 1);
        onRetry();
        setIsRetrying(false);
      }, retryDelay);
      
      setRetryTimer(timer);
      return () => {
        clearTimeout(timer);
        setIsRetrying(false);
      };
    }
  }, [status, autoRetry, retryCount, maxRetries, onRetry, retryDelay]);

  // Reset retry count on success
  useEffect(() => {
    if (status === 'success') {
      setRetryCount(0);
    }
  }, [status]);

  const handleManualRetry = () => {
    if (retryTimer) {
      clearTimeout(retryTimer);
      setRetryTimer(null);
    }
    setRetryCount(prev => prev + 1);
    setIsRetrying(false);
    if (onRetry) {
      onRetry();
    }
  };

  const getStatusIcon = () => {
    switch (status) {
      case 'running':
        return <InfoIcon color="info" />;
      case 'success':
        return <SuccessIcon color="success" />;
      case 'error':
        return <ErrorIcon color="error" />;
      case 'warning':
        return <WarningIcon color="warning" />;
      default:
        return null;
    }
  };

  const getStatusColor = () => {
    switch (status) {
      case 'running':
        return 'info';
      case 'success':
        return 'success';
      case 'error':
        return 'error';
      case 'warning':
        return 'warning';
      default:
        return 'default';
    }
  };

  const getStatusText = () => {
    switch (status) {
      case 'idle':
        return 'Ready to Test';
      case 'running':
        return 'Running Tests...';
      case 'success':
        return 'Tests Passed';
      case 'error':
        return 'Tests Failed';
      case 'warning':
        return 'Tests Completed with Warnings';
      default:
        return 'Unknown Status';
    }
  };

  if (!visible) {
    return null;
  }

  return (
    <Card 
      variant="outlined" 
      sx={{ 
        border: status !== 'idle' ? `2px solid` : undefined,
        borderColor: status !== 'idle' ? `${getStatusColor()}.main` : undefined,
        mb: 2
      }}
    >
      <CardContent>
        <Stack spacing={2}>
          {/* Header */}
          <Box display="flex" alignItems="center" justifyContent="space-between">
            <Box display="flex" alignItems="center" gap={2}>
              {getStatusIcon()}
              <Typography variant="h6">
                {testName}
              </Typography>
              <Chip
                size="small"
                label={getStatusText()}
                color={getStatusColor()}
                variant={status === 'idle' ? 'outlined' : 'filled'}
              />
            </Box>
            
            <Box display="flex" alignItems="center" gap={1}>
              {showControls && (
                <>
                  {status === 'idle' && onStart && (
                    <Tooltip title="Start Test">
                      <IconButton
                        color="primary"
                        onClick={onStart}
                        size="small"
                      >
                        <PlayIcon />
                      </IconButton>
                    </Tooltip>
                  )}
                  
                  {status === 'running' && onStop && (
                    <Tooltip title="Stop Test">
                      <IconButton
                        color="error"
                        onClick={onStop}
                        size="small"
                      >
                        <StopIcon />
                      </IconButton>
                    </Tooltip>
                  )}
                  
                  {(status === 'error' || status === 'warning') && onRetry && (
                    <Tooltip title={retryCount >= maxRetries ? "Max retries reached" : "Retry Test"}>
                      <IconButton
                        color="primary"
                        onClick={handleManualRetry}
                        size="small"
                        disabled={retryCount >= maxRetries}
                      >
                        <RefreshIcon />
                      </IconButton>
                    </Tooltip>
                  )}
                </>
              )}
              
              {(error || status === 'error') && (
                <IconButton
                  size="small"
                  onClick={() => setExpanded(!expanded)}
                >
                  {expanded ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                </IconButton>
              )}
            </Box>
          </Box>

          {/* Progress Bar */}
          {status === 'running' && (
            <Box>
              <LinearProgress 
                variant={progress > 0 ? "determinate" : "indeterminate"}
                value={progress}
                color="primary"
              />
              {progress > 0 && (
                <Typography variant="caption" color="text.secondary" sx={{ mt: 1 }}>
                  Progress: {Math.round(progress)}%
                </Typography>
              )}
            </Box>
          )}

          {/* Status Message */}
          {message && (
            <Alert 
              severity={getStatusColor()} 
              variant="outlined"
              sx={{ backgroundColor: 'transparent' }}
            >
              {message}
              {retryCount > 0 && (
                <Typography variant="caption" display="block" sx={{ mt: 0.5, opacity: 0.7 }}>
                  Retry attempt: {retryCount}/{maxRetries}
                </Typography>
              )}
              {isRetrying && (
                <Typography variant="caption" display="block" sx={{ mt: 0.5, opacity: 0.7 }}>
                  Auto-retrying in {Math.ceil(retryDelay / 1000)} seconds...
                </Typography>
              )}
            </Alert>
          )}

          {/* Error Details */}
          {error && (
            <Collapse in={expanded}>
              <ErrorDisplay
                error={error}
                title="Test Error Details"
                severity="error"
                showDetails={true}
                onRetry={onRetry}
                collapsible={false}
              />
            </Collapse>
          )}

          {/* Quick Actions */}
          {status === 'success' && (
            <Box display="flex" gap={1}>
              <Button
                size="small"
                variant="outlined"
                color="success"
                disabled
              >
                ✅ All Tests Passed
              </Button>
            </Box>
          )}
          
          {status === 'error' && onRetry && (
            <Box display="flex" gap={1} flexWrap="wrap">
              <Button
                size="small"
                variant="contained"
                color="primary"
                startIcon={<RefreshIcon />}
                onClick={handleManualRetry}
                disabled={retryCount >= maxRetries}
              >
                {retryCount >= maxRetries ? 'Max Retries Reached' : 'Retry Failed Tests'}
              </Button>
              
              {retryCount >= maxRetries && (
                <Button
                  size="small"
                  variant="outlined"
                  color="secondary"
                  onClick={() => {
                    setRetryCount(0);
                    if (onRetry) onRetry();
                  }}
                >
                  Force Retry
                </Button>
              )}
              
              {autoRetry && isRetrying && (
                <Button
                  size="small"
                  variant="outlined"
                  color="warning"
                  onClick={() => {
                    if (retryTimer) {
                      clearTimeout(retryTimer);
                      setRetryTimer(null);
                    }
                    setIsRetrying(false);
                  }}
                >
                  Cancel Auto-Retry
                </Button>
              )}
            </Box>
          )}
        </Stack>
      </CardContent>
    </Card>
  );
};

export default TestStatusIndicator;