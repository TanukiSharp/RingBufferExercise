# Overview

This project is an implementation of an exercise we ask to candidates during interview.<br/>
It is a ring buffer, with write priority, meaning the writer overwrites unread data, if any.<br/>
The candidate does not need to make it thread safe.

## Modes

The current implementation has a regular mode and an interactive mode.

The regular mode just prints all read and write operations in a single run, and the process ends.

In interactive mode, the program prints one operation, then waits for the user to press a key, then clears the console, and iterate over to the next operation, and so on.<br/>
This mode kind of show subsequent operations like an animation, except when the console flickers.

## Interface

The `RingBuffer` class is publicly defined as follow:

```cs
public class RingBuffer<T>
{
    public void Write(T? item);
    public bool TryRead(out T? result);
}
```

Because we want the buffer to have a fixed size, and the storage access to be as fast as possible, the implementation uses an array, and so the constructor takes a `size` argument.

## Prints

The `RingBuffer` class has a `ToString()` method that returns a prettified string representation of the ring buffer, with indexes and values, as well as the read and write pointers.

It also has a `ToString(bool showIndexes)` overload in case you want to hide the indexes line, since it can sometimes be confusing when focusing on the values.<br/>
Indexes are printed by default.

At the beginning of the application, the initial state of the ring buffer is printed:

```
Initial state

  R
| 0 | 1 | 2 | 3 | 4 |
---------------------
| 0 | 0 | 0 | 0 | 0 |
  W
```

When the buffer is read but has no data available, it prints the following:

```
Read: Could not read

  R
| 0 | 1 | 2 | 3 | 4 |
---------------------
| 0 | 0 | 0 | 0 | 0 |
  W
```

When writing 51, it prints the following:

Note that it stores the value first (`51`), and then advances the write pointer, therefore the `W` mark looks off.

```
Write: 51

  R
| 0  | 1  | 2  | 3  | 4  |
--------------------------
| 51 | 0  | 0  | 0  | 0  |
       W
```

When reading, it prints the following:

Note that same as for writing, reading fetches the value first (`53`), and then advances the read pointer.<br/>

```
# Previous state:

            R
| 0  | 1  | 2  | 3  | 4  |
--------------------------
| 56 | 57 | 53 | 54 | 55 |
            W

Read: 53

                 R
| 0  | 1  | 2  | 3  | 4  |
--------------------------
| 56 | 57 | 53 | 54 | 55 |
            W
```

## Edge case behaviors

### Nothing to read

When there is nothing to read, the `TryRead` method returns `false` to indicate it could not read anything.

### Overwrite

When there are data available to read, but the reader did not consume enough and the writer is about to write, old data is overwritten, and the write pushes the read pointer one item away, as follow:

```
Write: 56

       R
| 0  | 1  | 2  | 3  | 4  |
--------------------------
| 56 | 52 | 53 | 54 | 55 |
       W


Write: 57

            R
| 0  | 1  | 2  | 3  | 4  |
--------------------------
| 56 | 57 | 53 | 54 | 55 |
            W
```

You can see that when writing `57`, the value `52` has been overwritten and will not be available to the reader anymore, and so the next read will get the value `53`.
