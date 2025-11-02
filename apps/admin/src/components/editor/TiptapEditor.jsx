'use client';

import React from 'react';
import { useEditor, EditorContent } from '@tiptap/react';
import StarterKit from '@tiptap/starter-kit';
import Placeholder from '@tiptap/extension-placeholder';
import { Box, Paper, IconButton, Divider, Tooltip } from '@mui/material';
import {
  FormatBold as BoldIcon,
  FormatItalic as ItalicIcon,
  FormatListBulleted as BulletListIcon,
  FormatListNumbered as NumberedListIcon,
  FormatQuote as BlockquoteIcon,
  Code as CodeIcon,
  Undo as UndoIcon,
  Redo as RedoIcon
} from '@mui/icons-material';

const MenuBar = ({ editor }) => {
  if (!editor) {
    return null;
  }

  const buttonStyle = (isActive) => ({
    color: isActive ? 'primary.main' : 'inherit',
    backgroundColor: isActive ? 'action.selected' : 'transparent',
    '&:hover': {
      backgroundColor: 'action.hover'
    }
  });

  return (
    <Box sx={{ display: 'flex', gap: 0.5, flexWrap: 'wrap', p: 1, borderBottom: 1, borderColor: 'divider' }}>
      <Tooltip title="Bold (Ctrl+B)">
        <IconButton
          size="small"
          onClick={() => editor.chain().focus().toggleBold().run()}
          sx={buttonStyle(editor.isActive('bold'))}
        >
          <BoldIcon fontSize="small" />
        </IconButton>
      </Tooltip>

      <Tooltip title="Italic (Ctrl+I)">
        <IconButton
          size="small"
          onClick={() => editor.chain().focus().toggleItalic().run()}
          sx={buttonStyle(editor.isActive('italic'))}
        >
          <ItalicIcon fontSize="small" />
        </IconButton>
      </Tooltip>

      <Divider orientation="vertical" flexItem sx={{ mx: 0.5 }} />

      <Tooltip title="Bullet List">
        <IconButton
          size="small"
          onClick={() => editor.chain().focus().toggleBulletList().run()}
          sx={buttonStyle(editor.isActive('bulletList'))}
        >
          <BulletListIcon fontSize="small" />
        </IconButton>
      </Tooltip>

      <Tooltip title="Numbered List">
        <IconButton
          size="small"
          onClick={() => editor.chain().focus().toggleOrderedList().run()}
          sx={buttonStyle(editor.isActive('orderedList'))}
        >
          <NumberedListIcon fontSize="small" />
        </IconButton>
      </Tooltip>

      <Divider orientation="vertical" flexItem sx={{ mx: 0.5 }} />

      <Tooltip title="Blockquote">
        <IconButton
          size="small"
          onClick={() => editor.chain().focus().toggleBlockquote().run()}
          sx={buttonStyle(editor.isActive('blockquote'))}
        >
          <BlockquoteIcon fontSize="small" />
        </IconButton>
      </Tooltip>

      <Tooltip title="Code Block">
        <IconButton
          size="small"
          onClick={() => editor.chain().focus().toggleCodeBlock().run()}
          sx={buttonStyle(editor.isActive('codeBlock'))}
        >
          <CodeIcon fontSize="small" />
        </IconButton>
      </Tooltip>

      <Divider orientation="vertical" flexItem sx={{ mx: 0.5 }} />

      <Tooltip title="Undo (Ctrl+Z)">
        <IconButton
          size="small"
          onClick={() => editor.chain().focus().undo().run()}
          disabled={!editor.can().undo()}
        >
          <UndoIcon fontSize="small" />
        </IconButton>
      </Tooltip>

      <Tooltip title="Redo (Ctrl+Y)">
        <IconButton
          size="small"
          onClick={() => editor.chain().focus().redo().run()}
          disabled={!editor.can().redo()}
        >
          <RedoIcon fontSize="small" />
        </IconButton>
      </Tooltip>
    </Box>
  );
};

const TiptapEditor = ({ content, onChange, placeholder = 'Start typing...', minHeight = 200 }) => {
  const editor = useEditor({
    extensions: [
      StarterKit,
      Placeholder.configure({
        placeholder
      })
    ],
    content,
    immediatelyRender: false, // Fix SSR hydration mismatch
    onUpdate: ({ editor }) => {
      const html = editor.getHTML();
      onChange(html);
    },
    editorProps: {
      attributes: {
        class: 'tiptap-editor'
      }
    }
  });

  // Return null during SSR
  if (!editor) {
    return null;
  }

  return (
    <Paper variant="outlined" sx={{ borderRadius: 1 }}>
      <MenuBar editor={editor} />
      <Box
        sx={{
          minHeight,
          maxHeight: 400,
          overflowY: 'auto',
          p: 2,
          '& .tiptap-editor': {
            outline: 'none',
            '& p.is-editor-empty:first-of-type::before': {
              color: 'text.disabled',
              content: 'attr(data-placeholder)',
              float: 'left',
              height: 0,
              pointerEvents: 'none'
            },
            '& p': {
              margin: '0.5em 0'
            },
            '& ul, & ol': {
              paddingLeft: '1.5rem',
              margin: '0.5em 0'
            },
            '& blockquote': {
              borderLeft: '3px solid',
              borderColor: 'divider',
              paddingLeft: '1rem',
              margin: '1em 0',
              color: 'text.secondary'
            },
            '& pre': {
              backgroundColor: 'action.hover',
              borderRadius: 1,
              padding: '0.75rem 1rem',
              margin: '1em 0',
              fontFamily: 'monospace',
              fontSize: '0.875rem',
              overflowX: 'auto'
            },
            '& code': {
              backgroundColor: 'action.hover',
              borderRadius: '0.25rem',
              padding: '0.125rem 0.25rem',
              fontFamily: 'monospace',
              fontSize: '0.875rem'
            },
            '& h1, & h2, & h3, & h4, & h5, & h6': {
              margin: '1em 0 0.5em',
              fontWeight: 600
            }
          }
        }}
      >
        <EditorContent editor={editor} />
      </Box>
    </Paper>
  );
};

export default TiptapEditor;
