'use client';

import React, { useState } from 'react';
import {
  Box,
  Button,
  Card,
  CardContent,
  TextField,
  Typography,
  IconButton,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  FormControlLabel,
  Checkbox,
  Grid,
  Chip,
  Stack,
  Alert
} from '@mui/material';
import {
  Add as AddIcon,
  Delete as DeleteIcon,
  ExpandMore as ExpandMoreIcon,
  Edit as EditIcon,
  Save as SaveIcon,
  Cancel as CancelIcon
} from '@mui/icons-material';

export default function ItineraryManager({ tourId, initialItinerary = [], tourType = 'tour', onChange }) {
  const [itinerary, setItinerary] = useState(initialItinerary);
  const [editingId, setEditingId] = useState(null);
  const [newDay, setNewDay] = useState({
    dayNumber: itinerary.length + 1,
    header: '',
    textDetail: '',
    activities: '',
    meals: { breakfast: false, lunch: false, dinner: false },
    accommodation: ''
  });

  const handleAddDay = () => {
    const dayData = {
      ...newDay,
      activities: newDay.activities.split('\n').filter(a => a.trim())
    };

    const updatedItinerary = [...itinerary, dayData].sort((a, b) => a.dayNumber - b.dayNumber);
    setItinerary(updatedItinerary);
    onChange?.(updatedItinerary);

    // Reset form
    setNewDay({
      dayNumber: updatedItinerary.length + 1,
      header: '',
      textDetail: '',
      activities: '',
      meals: { breakfast: false, lunch: false, dinner: false },
      accommodation: ''
    });
  };

  const handleUpdateDay = (id, updatedData) => {
    const updatedItinerary = itinerary.map(day =>
      day._id === id ? { ...day, ...updatedData } : day
    );
    setItinerary(updatedItinerary);
    onChange?.(updatedItinerary);
    setEditingId(null);
  };

  const handleDeleteDay = (id) => {
    const updatedItinerary = itinerary.filter(day => day._id !== id);
    setItinerary(updatedItinerary);
    onChange?.(updatedItinerary);
  };

  if (tourType === 'daytrip') {
    return (
      <Alert severity="info">
        Itinerary is only available for multi-day tours. Change tour type to &quot;Tour&quot; to add itinerary.
      </Alert>
    );
  }

  return (
    <Box>
      <Typography variant="h6" gutterBottom>
        Tour Itinerary
      </Typography>

      {/* Existing Itinerary Days */}
      {itinerary.map((day, index) => (
        <Accordion key={day._id || index} sx={{ mb: 2 }}>
          <AccordionSummary expandIcon={<ExpandMoreIcon />}>
            <Box sx={{ display: 'flex', alignItems: 'center', width: '100%', gap: 2 }}>
              <Chip label={`Day ${day.dayNumber}`} color="primary" />
              <Typography sx={{ flexGrow: 1 }}>{day.header}</Typography>
              <Box sx={{ display: 'flex', gap: 1 }}>
                {day.meals?.breakfast && <Chip label="B" size="small" />}
                {day.meals?.lunch && <Chip label="L" size="small" />}
                {day.meals?.dinner && <Chip label="D" size="small" />}
              </Box>
            </Box>
          </AccordionSummary>
          <AccordionDetails>
            {editingId === day._id ? (
              <EditDayForm
                day={day}
                onSave={(data) => handleUpdateDay(day._id, data)}
                onCancel={() => setEditingId(null)}
              />
            ) : (
              <Box>
                <Typography variant="body2" paragraph>
                  {day.textDetail}
                </Typography>

                {day.activities && day.activities.length > 0 && (
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="subtitle2" gutterBottom>Activities:</Typography>
                    <ul>
                      {day.activities.map((activity, i) => (
                        <li key={i}><Typography variant="body2">{activity}</Typography></li>
                      ))}
                    </ul>
                  </Box>
                )}

                {day.accommodation && (
                  <Box sx={{ mb: 2 }}>
                    <Typography variant="subtitle2" gutterBottom>Accommodation:</Typography>
                    <Typography variant="body2">{day.accommodation}</Typography>
                  </Box>
                )}

                <Stack direction="row" spacing={1} sx={{ mt: 2 }}>
                  <Button
                    startIcon={<EditIcon />}
                    onClick={() => setEditingId(day._id)}
                    size="small"
                  >
                    Edit
                  </Button>
                  <Button
                    startIcon={<DeleteIcon />}
                    onClick={() => handleDeleteDay(day._id)}
                    color="error"
                    size="small"
                  >
                    Delete
                  </Button>
                </Stack>
              </Box>
            )}
          </AccordionDetails>
        </Accordion>
      ))}

      {/* Add New Day Form */}
      <Card sx={{ mt: 3 }}>
        <CardContent>
          <Typography variant="h6" gutterBottom>
            Add New Day
          </Typography>

          <Grid container spacing={2}>
            <Grid item xs={12} sm={3}>
              <TextField
                fullWidth
                label="Day Number"
                type="number"
                value={newDay.dayNumber}
                onChange={(e) => setNewDay({ ...newDay, dayNumber: parseInt(e.target.value) })}
                InputProps={{ inputProps: { min: 1 } }}
              />
            </Grid>

            <Grid item xs={12} sm={9}>
              <TextField
                fullWidth
                label="Day Header"
                value={newDay.header}
                onChange={(e) => setNewDay({ ...newDay, header: e.target.value })}
                placeholder="e.g., Arrival in Paris - City Tour"
                required
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                multiline
                rows={4}
                label="Day Details"
                value={newDay.textDetail}
                onChange={(e) => setNewDay({ ...newDay, textDetail: e.target.value })}
                placeholder="Describe what happens on this day..."
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                multiline
                rows={3}
                label="Activities (one per line)"
                value={newDay.activities}
                onChange={(e) => setNewDay({ ...newDay, activities: e.target.value })}
                placeholder="Visit Eiffel Tower&#10;Seine River Cruise&#10;Dinner at local restaurant"
              />
            </Grid>

            <Grid item xs={12}>
              <Typography variant="subtitle2" gutterBottom>
                Meals Included:
              </Typography>
              <Stack direction="row" spacing={2}>
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={newDay.meals.breakfast}
                      onChange={(e) => setNewDay({
                        ...newDay,
                        meals: { ...newDay.meals, breakfast: e.target.checked }
                      })}
                    />
                  }
                  label="Breakfast"
                />
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={newDay.meals.lunch}
                      onChange={(e) => setNewDay({
                        ...newDay,
                        meals: { ...newDay.meals, lunch: e.target.checked }
                      })}
                    />
                  }
                  label="Lunch"
                />
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={newDay.meals.dinner}
                      onChange={(e) => setNewDay({
                        ...newDay,
                        meals: { ...newDay.meals, dinner: e.target.checked }
                      })}
                    />
                  }
                  label="Dinner"
                />
              </Stack>
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Accommodation"
                value={newDay.accommodation}
                onChange={(e) => setNewDay({ ...newDay, accommodation: e.target.value })}
                placeholder="e.g., 4-star hotel in city center"
              />
            </Grid>

            <Grid item xs={12}>
              <Button
                variant="contained"
                startIcon={<AddIcon />}
                onClick={handleAddDay}
                disabled={!newDay.header}
              >
                Add Day
              </Button>
            </Grid>
          </Grid>
        </CardContent>
      </Card>
    </Box>
  );
}

function EditDayForm({ day, onSave, onCancel }) {
  const [formData, setFormData] = useState({
    ...day,
    activities: Array.isArray(day.activities) ? day.activities.join('\n') : ''
  });

  const handleSave = () => {
    onSave({
      ...formData,
      activities: formData.activities.split('\n').filter(a => a.trim())
    });
  };

  return (
    <Grid container spacing={2}>
      <Grid item xs={12} sm={3}>
        <TextField
          fullWidth
          label="Day Number"
          type="number"
          value={formData.dayNumber}
          onChange={(e) => setFormData({ ...formData, dayNumber: parseInt(e.target.value) })}
        />
      </Grid>

      <Grid item xs={12} sm={9}>
        <TextField
          fullWidth
          label="Day Header"
          value={formData.header}
          onChange={(e) => setFormData({ ...formData, header: e.target.value })}
        />
      </Grid>

      <Grid item xs={12}>
        <TextField
          fullWidth
          multiline
          rows={4}
          label="Day Details"
          value={formData.textDetail}
          onChange={(e) => setFormData({ ...formData, textDetail: e.target.value })}
        />
      </Grid>

      <Grid item xs={12}>
        <TextField
          fullWidth
          multiline
          rows={3}
          label="Activities"
          value={formData.activities}
          onChange={(e) => setFormData({ ...formData, activities: e.target.value })}
        />
      </Grid>

      <Grid item xs={12}>
        <Stack direction="row" spacing={2}>
          <FormControlLabel
            control={
              <Checkbox
                checked={formData.meals?.breakfast || false}
                onChange={(e) => setFormData({
                  ...formData,
                  meals: { ...formData.meals, breakfast: e.target.checked }
                })}
              />
            }
            label="Breakfast"
          />
          <FormControlLabel
            control={
              <Checkbox
                checked={formData.meals?.lunch || false}
                onChange={(e) => setFormData({
                  ...formData,
                  meals: { ...formData.meals, lunch: e.target.checked }
                })}
              />
            }
            label="Lunch"
          />
          <FormControlLabel
            control={
              <Checkbox
                checked={formData.meals?.dinner || false}
                onChange={(e) => setFormData({
                  ...formData,
                  meals: { ...formData.meals, dinner: e.target.checked }
                })}
              />
            }
            label="Dinner"
          />
        </Stack>
      </Grid>

      <Grid item xs={12}>
        <TextField
          fullWidth
          label="Accommodation"
          value={formData.accommodation}
          onChange={(e) => setFormData({ ...formData, accommodation: e.target.value })}
        />
      </Grid>

      <Grid item xs={12}>
        <Stack direction="row" spacing={1}>
          <Button
            variant="contained"
            startIcon={<SaveIcon />}
            onClick={handleSave}
          >
            Save
          </Button>
          <Button
            variant="outlined"
            startIcon={<CancelIcon />}
            onClick={onCancel}
          >
            Cancel
          </Button>
        </Stack>
      </Grid>
    </Grid>
  );
}
