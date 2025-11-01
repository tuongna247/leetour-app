# Bible Verse Selector - Improved Version

## Overview
The improved Bible verse selector now supports:
- Loading Bible data from external `bible.json` file via AJAX
- Filtering which books appear in the dropdown using `ALLOWED_BOOKS` array
- Fallback to embedded data if JSON file is not found

## Files

### 1. bible-verse-selector-improved.html
Main HTML file with the improved selector implementation.

### 2. bible.json.example
Example structure for your Bible data JSON file.

## How to Use

### Step 1: Prepare Your Bible Data

Rename `bible.json.example` to `bible.json` and populate it with your complete Bible data:

```json
{
  "structure": {
    "Book Name": {
      "chapter_number": verse_count,
      "1": 25,
      "2": 23
    }
  },
  "verses": {
    "Book Name": {
      "chapter_number": {
        "verse_number": "verse text",
        "1": "Verse text here",
        "2": "Another verse text"
      }
    }
  }
}
```

**structure**: Contains book names with chapter numbers and how many verses each chapter has
**verses**: (Optional) Contains the actual verse text content

### Step 2: Configure Which Books to Show

Edit the `ALLOWED_BOOKS` array in `bible-verse-selector-improved.html` (around line 454):

```javascript
const ALLOWED_BOOKS = [
    'Ma-thi-ơ',      // Matthew
    'Mác',           // Mark
    'Lu-ca',         // Luke
    'Giăng',         // John
    'Công Vụ',       // Acts
    'Rô-ma',         // Romans
    // Add more book names as needed
];
```

**To show ALL books**: Leave the array empty `const ALLOWED_BOOKS = [];`
**To show specific books**: Add only the book names you want to display

### Step 3: Load from API (Optional)

If you want to load from an API endpoint instead of a local file, uncomment and modify lines 499-502:

```javascript
// Option 2: Load from API endpoint
$.getJSON('https://your-api.com/bible', function(data) {
    bibleData = data;
    initializeBooks();
});
```

### Step 4: Run the HTML File

Simply open `bible-verse-selector-improved.html` in a web browser. The selector will:
1. Try to load `bible.json` from the same directory
2. If successful, use that data
3. If failed, fall back to embedded sample data
4. Only show books listed in `ALLOWED_BOOKS` array

## Features

- **Verse hover tooltips**: Hover over any verse number to see its content in a tooltip
- **Multiple verse selection**: Click verses to select/deselect
- **Quick selection shortcuts**:
  - "Chọn toàn bộ đoạn" - Select entire chapter
  - "Bỏ chọn tất cả" - Deselect all
- **Visual feedback**: Selected verses are highlighted
- **Smart tooltip positioning**: Tooltips automatically adjust to stay on screen
- **Responsive design**: Works on desktop and mobile
- **Fallback support**: Works even without bible.json file

## Example Usage

```javascript
// Get selected verses programmatically
const selector = window.bibleSelector;
const selected = selector.getSelectedVerses();
console.log(selected);
// Output: [
//   { book: "Ma-thi-ơ", chapter: 5, verse: 3 },
//   { book: "Ma-thi-ơ", chapter: 5, verse: 4 }
// ]
```

## Notes

- The book names in `ALLOWED_BOOKS` must exactly match the book names in your `bible.json` file
- If using Vietnamese Bible, ensure your JSON file uses UTF-8 encoding
- The selector requires jQuery 3.6.0+ (already included via CDN)
